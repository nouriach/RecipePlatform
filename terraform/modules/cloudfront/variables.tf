variable "s3_bucket_name" {
  type        = string
  description = "Name of the S3 bucket used as the CloudFront origin."
}

variable "s3_bucket_domain_name" {
  type        = string
  description = "Bucket domain name for the S3 origin."
}

variable "oac_id" {
  type        = string
  description = "CloudFront Origin Access Control ID used to access the S3 bucket."
}

variable "default_root_object" {
  type        = string
  description = "Default file served for root requests."
  default     = "index.html"
}

variable "price_class" {
  type        = string
  description = "CloudFront price class."
  default     = "PriceClass_100"
}

variable "aliases" {
  type        = list(string)
  description = "Optional domain aliases for the distribution."
  default     = []
}

variable "certificate_arn" {
  type        = string
  description = "ACM certificate ARN for custom domain support. Required if aliases are provided."
  default     = null
}

variable "enable_logging" {
  type        = bool
  description = "Enable CloudFront access logging."
  default     = false
}

variable "log_bucket" {
  type        = string
  description = "S3 bucket for CloudFront logs (required if enable_logging = true)."
  default     = null
}
