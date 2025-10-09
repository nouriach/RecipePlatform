resource "null_resource" "build_dotnet_lambda" {
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