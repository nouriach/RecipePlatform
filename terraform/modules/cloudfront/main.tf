data "aws_cloudfront_cache_policy" "caching_optimized" {
  name = "Managed-CachingOptimized"
}

data "aws_cloudfront_origin_request_policy" "cors_s3" {
  name = "Managed-CORS-S3Origin"
}

data "aws_cloudfront_response_headers_policy" "security_headers" {
  name = "Managed-SecurityHeadersPolicy"
}

resource "aws_cloudfront_origin_access_control" "webapp_oac" {
  name                              = "webapp-oac"
  description                       = "OAC for Blazor web app"
  signing_behavior                  = "always"
  signing_protocol                  = "sigv4"
  origin_access_control_origin_type = "s3"
}


resource "aws_cloudfront_distribution" "this" {
  origin {
    domain_name = var.s3_bucket_domain_name
    origin_id   = "s3-origin-${var.s3_bucket_name}"

    origin_access_control_id = aws_cloudfront_origin_access_control.webapp_oac.id
  }

  default_cache_behavior {
    allowed_methods = ["GET", "HEAD"]
    cached_methods  = ["GET", "HEAD"]
    target_origin_id       = "s3-origin-${var.s3_bucket_name}"
    viewer_protocol_policy = "redirect-to-https"

    cache_policy_id            = data.aws_cloudfront_cache_policy.caching_optimized.id
    origin_request_policy_id   = data.aws_cloudfront_origin_request_policy.cors_s3.id
    response_headers_policy_id = data.aws_cloudfront_response_headers_policy.security_headers.id
  }

  custom_error_response { # If the S3 request returns a 403, redict to the route.
    error_code            = 403
    response_code         = 200
    response_page_path    = "/index.html"
    error_caching_min_ttl = 0
  }
  
  default_root_object = var.default_root_object

  enabled             = true
  
  restrictions {
    geo_restriction {
      restriction_type = "none"
      locations = []
    }
  }

  viewer_certificate {
    cloudfront_default_certificate = var.certificate_arn == null
    acm_certificate_arn            = var.certificate_arn
    ssl_support_method             = var.certificate_arn == null ? null : "sni-only"
  }
}