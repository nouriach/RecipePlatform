variable "lambda_name" {
  type    = string
  default = ""
}

variable "runtime" {
  type    = string
  default = "dotnet8"
}

variable "memory_size" {
  type    = number
  default = 512
}

variable "timeout" {
  type    = number
  default = 15
}

variable "project_path" {
  type = string
  default = ""
}

variable "lambda_handler" {
  type = string
  default = ""
}

variable "build_trigger" {
  type = any
}

variable "role_name" {
  type = string
}

variable "cloudwatch_policy_name" {
  type = string
}

variable "s3_policy_name" {
  type = string
}

variable "dynamodb_policy_name" {
  type = string
}

variable "ssm_policy_name" {
  type = string
}

variable "ses_policy_name" {
  type = string
}

variable "allowed_origin" {
  type = string
  default = ""
}