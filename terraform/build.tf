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
      dotnet restore "${path.root}/../src/RecipePlatform.NotificationService/RecipePlatform.NotificationService.csproj"
      dotnet publish "${path.root}/../src/RecipePlatform.NotificationService/RecipePlatform.NotificationService.csproj" \
        -c Release \
        -r linux-x64 \
        --self-contained false \
        -o "${path.root}/../src/RecipePlatform.NotificationService/publish"
    EOT
    interpreter = ["/bin/sh", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }
}