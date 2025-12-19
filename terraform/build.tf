resource "null_resource" "build_datamanager_lambda" {
  provisioner "local-exec" {
    command     = <<EOT
      dotnet restore "${path.root}/../src/RecipePlatform.DataManager/RecipePlatform.DataManager.csproj"
      dotnet publish "${path.root}/../src/RecipePlatform.DataManager/RecipePlatform.DataManager.csproj" \
        -c Release \
        -r linux-x64 \
        --self-contained false \
        -o "${path.root}/../src/RecipePlatform.DataManager/publish"
    EOT
    interpreter = ["/bin/sh", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }
}

resource "null_resource" "build_notificationservice_lambda" {
  provisioner "local-exec" {
    command     = <<EOT
      rm -rf "${path.root}/../publish/NotificationService"
      dotnet restore "${path.root}/../src/RecipePlatform.NotificationService/RecipePlatform.NotificationService.csproj"
      dotnet publish "${path.root}/../src/RecipePlatform.NotificationService/RecipePlatform.NotificationService.csproj" \
        -c Release \
        -r linux-x64 \
        --self-contained false \
        -o "${path.root}/../publish/NotificationService"
    EOT
    interpreter = ["/bin/sh", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }
}

resource "null_resource" "build_webapp" {
  provisioner "local-exec" {
    command = <<EOT
      rm -rf ../src/RecipePlatform.WebApp/build
      dotnet publish ../src/RecipePlatform.WebApp -c Release -o ../src/RecipePlatform.WebApp/build
    EOT
  }
  
  triggers = {
    always_run = timestamp()
  }
}

resource "null_resource" "deploy_webapp" {
  depends_on = [null_resource.build_webapp]

  provisioner "local-exec" {
    command = <<EOT
      aws s3 sync \
        ../src/RecipePlatform.WebApp/build/wwwroot \
        s3://${module.web_app_s3.bucket_name} \
        --delete

      aws cloudfront create-invalidation \
        --distribution-id ${module.web_app_cdn.distribution_id} \
        --paths "/*"
    EOT
  }
  
  triggers = {
    always_run = timestamp()
  }
}