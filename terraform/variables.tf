variable "aws_profile" {
  description = "Given name in the credential file"
  type        = string
  default     = ""
}

variable "region" {
  type    = string
  default = "eu-west-2"
}

variable "data_manager_name" {
  type    = string
  default = "RecipePlatform.DataManager"
}

variable "notification_service_name" {
  type    = string
  default = "RecipePlatform.NotificationService"
}

variable "scheduler_name" {
  type    = string
  default = "RecipePlatform.Scheduler"
}
