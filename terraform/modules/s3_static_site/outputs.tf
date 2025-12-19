output "bucket_id" {
  value       = aws_s3_bucket.this.id
  description = "The ID of the S3 bucket."
}

output "bucket_arn" {
  value       = aws_s3_bucket.this.arn
  description = "The ARN of the S3 bucket."
}

output "bucket_name" {
  value       = aws_s3_bucket.this.bucket
  description = "The name of the S3 bucket."
}

output "bucket_domain_name" {
  value       = aws_s3_bucket.this.bucket_domain_name
  description = "The domain name of the S3 bucket (used by CloudFront as the origin)."
}

output "oac_id" {
  value       = aws_cloudfront_origin_access_control.oac.id
  description = "ID of the CloudFront Origin Access Control associated with this bucket."
}

output "website_endpoint" {
  value       = aws_s3_bucket_website_configuration.this.website_endpoint
  description = "S3 static website hosting endpoint (not used when CloudFront is in front)."
}
