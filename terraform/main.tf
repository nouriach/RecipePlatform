module 'data_manager_lambda' {
  source           = "./modules/lambda"
  lambda_name      = "RecipePlatform.DataManager"
  runtime          = "dotnet8"
  timeout          = 15
  memory_size      = 512
  project_path     = "../src/RecipePlatform.DataManager"
}

output "data_manager_lambda_url" {
  value = module.data_manager_lambda.function_url
}
