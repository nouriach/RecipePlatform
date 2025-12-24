output "distribution_id" {
  value       = aws_cloudfront_distribution.this.id
  description = "ID of the CloudFront distribution."
}

output "distribution_arn" {
  value       = aws_cloudfront_distribution.this.arn
  description = "ARN of the CloudFront distribution."
}

output "domain_name" {
  value       = aws_cloudfront_distribution.this.domain_name
  description = "CloudFront domain name for accessing the distribution."
}

output "hosted_zone_id" {
  value       = aws_cloudfront_distribution.this.hosted_zone_id
  description = "Route53 Hosted Zone ID for CloudFront alias records."
}

output "oac_id" {
  value       = var.oac_id
  description = "Origin Access Control ID passed into this module."
}