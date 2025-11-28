module "data_manager_lambda" {
  source         = "./modules/lambda"
  lambda_name    = "recipeplatform-datamanager"
  runtime        = "dotnet8"
  lambda_handler = "RecipePlatform.DataManager"
  timeout        = 15
  memory_size    = 512
  project_path   = "${path.root}/../src/RecipePlatform.DataManager"
  role_name      = "data-manager-lambda"
  cloudwatch_policy_name = "data-manager-cloudwatch"
  s3_policy_name = "datamanager-s3"
  dynamodb_policy_name = "datamanager-dynamodb"
  ssm_policy_name = "datamanager-ssm"
  ses_policy_name = "datamanager-ses"

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
  role_name      = "notification-service-lambda"
  cloudwatch_policy_name = "notificationservice-cloudwatch"
  s3_policy_name = "notificationservice-s3"
  dynamodb_policy_name = "notificationservice-dynamodb"
  ssm_policy_name = "notificationservice-ssm"
  ses_policy_name = "notificationservice-ses"

  build_trigger = null_resource.build_notificationservice_lambda
}

module "data_manager_s3" {
  source      = "./modules/S3"
  bucket_name = "weekly-recipe-recommendations"
  s3_policy_name = "notification-service-ssm"
}

output "data_manager_lambda_url" {
  value = module.data_manager_lambda.function_url
}