resource "aws_lambda_function" "this" {
  function_name = var.lambda_name
  runtime       = var.runtime
  memory_size   = var.memory_size
  timeout       = var.timeout
  role          = aws_iam_role.lambda_iam_role.arn

  filename         = data.archive_file.this.output_path
  source_code_hash = data.archive_file.this.output_base64sha256
  
  publish = true

  depends_on = [
    aws_iam_role_policy_attachment.lambda_logging_policy_attachment,
    aws_iam_role_policy_attachment.lambda_s3_policy_attachment
  ]
}

data "archive_file" "this" {
  type        = "zip"
  source_dir  = "${var.project_path}/publish"
  output_path = "${var.project_path}/lambda.zip"
}

resource "aws_lambda_function_url" "this" {
  function_name = aws_lambda_function.this.function_name
  authorization_type = "NONE"  # or "AWS_IAM" if you want restricted access
}