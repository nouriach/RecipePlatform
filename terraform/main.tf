module "data_manager_lambda" {
  source         = "./modules/lambda"
  lambda_name    = "recipeplatform-datamanager"
  runtime        = "dotnet8"
  lambda_handler = "RecipePlatform.DataManager"
  timeout        = 15
  memory_size    = 512
  project_path   = "${path.root}/../src/RecipePlatform.DataManager"
  role_name      = "datamanager_lambda"
  cloudwatch_policy_name = "datamanager_cloudwatch"
  s3_policy_name = "datamanager_s3"
  dynamodb_policy_name = "datamanager_dynamodb"
  ssm_policy_name = "datamanager_ssm"

  build_trigger = null_resource.build_datamanager_lambda
}

module "notification_service_lambda" {
  source         = "./modules/lambda"
  lambda_name    = "recipeplatform-notificationservice"
  runtime        = "dotnet8"
  lambda_handler = "RecipePlatform.NotificationService"
  timeout        = 15
  memory_size    = 512
  project_path   = "${path.root}/../src/RecipePlatform.NotificationService"
  role_name      = "notificationservice_lambda"
  cloudwatch_policy_name = "notificationservice_cloudwatch"
  s3_policy_name = "notificationservice_s3"
  dynamodb_policy_name = "notificationservice_dynamodb"
  ssm_policy_name = "notificationservice_ssm"

  build_trigger = null_resource.build_notificationservice_lambda
}

module "data_manager_s3" {
  source      = "./modules/S3"
  bucket_name = "weekly-recipe-recommendations"
}

output "data_manager_lambda_url" {
  value = module.data_manager_lambda.function_url
}
