module "data_manager_lambda" {
  source         = "./modules/lambda"
  lambda_name    = "recipeplatform-datamanager"
  runtime        = "dotnet8"
  lambda_handler = "RecipePlatform.DataManager"
  timeout        = 15
  memory_size    = 512
  project_path   = "${path.root}/../src/RecipePlatform.DataManager"

  build_trigger = null_resource.build_dotnet_lambda
}
module "data_manager_s3" {
  source      = "./modules/S3"
  bucket_name = "weekly-recipe-recommendations"
}

output "data_manager_lambda_url" {
  value = module.data_manager_lambda.function_url
}
