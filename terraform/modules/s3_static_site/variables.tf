variable "bucket_name" {
  type        = string
  description = "Name of the S3 bucket hosting the static site."
}

variable "block_public_access" {
  type        = bool
  description = "Whether to block all public access. Recommended true when using CloudFront OAC."
  default     = true
}

variable "create_oac" {
  type        = bool
  description = "Whether to create a CloudFront Origin Access Control for this bucket."
  default     = true
}

variable "versioning" {
  type        = bool
  description = "Enable versioning on the S3 bucket."
  default     = true
}

variable "index_document" {
  type        = string
  description = "Index document for the static website."
  default     = "index.html"
}

variable "error_document" {
  type        = string
  description = "Error document for the static website."
  default     = "index.html"
}
