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
  allowed_origin = module.web_app_cdn.domain_name
  build_trigger = null_resource.build_datamanager_lambda
}

module "notification_service_lambda" {
  source         = "./modules/lambda"
  lambda_name    = "recipeplatform-notificationservice"
  runtime        = "dotnet8"
  lambda_handler = "RecipePlatform.NotificationService"
  timeout        = 15
  memory_size    = 512
  project_path   = "${path.root}/../publish/NotificationService"
  role_name      = "notification-service-lambda"
  cloudwatch_policy_name = "notificationservice-cloudwatch"
  s3_policy_name = "notificationservice-s3"
  dynamodb_policy_name = "notificationservice-dynamodb"
  ssm_policy_name = "notificationservice-ssm"
  ses_policy_name = "notificationservice-ses"
  allowed_origin = module.web_app_cdn.domain_name
  
  build_trigger = null_resource.build_notificationservice_lambda
}

module "data_manager_s3" {
  source      = "./modules/s3"
  bucket_name = "weekly-recipe-recommendations"
  s3_policy_name = "notification-service-ssm"
}

module "web_app_s3" {
  source = "./modules/s3_static_site"

  bucket_name         = "recipe-platform-web-app"
  block_public_access = true
  create_oac          = true
}

module "web_app_cdn" {
  source = "./modules/cloudfront"

  s3_bucket_name       = module.web_app_s3.bucket_name
  s3_bucket_domain_name = module.web_app_s3.bucket_domain_name
  oac_id               = module.web_app_s3.oac_id

  default_root_object  = "index.html"
}

output "data_manager_lambda_url" {
  value = module.data_manager_lambda.function_url
}

# Bucket policy allowing CloudFront access (OAC)
resource "aws_s3_bucket_policy" "allow_cloudfront_oac" {
  bucket = module.web_app_s3.bucket_id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Sid    = "AllowCloudFrontAccess"
      Effect = "Allow"
      Principal = {
        Service = "cloudfront.amazonaws.com"
      }
      Action = ["s3:GetObject"]
      Resource = "${module.web_app_s3.bucket_arn}/*"
      Condition = {
        StringEquals = {
          "AWS:SourceArn" : module.web_app_cdn.distribution_arn
        }
      }
    }]
  })
}