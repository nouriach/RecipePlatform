data "aws_iam_policy_document" "lambda_assume_role_policy" {
  statement {
    effect = "Allow"
    principals {
      type        = "Service"
      identifiers = ["lambda.amazonaws.com"]
    }
    actions = [
      "sts:AssumeRole"
    ]
  }
}

data "aws_iam_policy_document" "lambda_allow_logging" {
  policy_id = "AllowLambdaPushLog"
  statement {
    effect = "Allow"
    actions = [
      "logs:CreateLogGroup",
      "logs:CreateLogStream",
      "logs:PutLogEvents",
    ]
    resources = ["arn:aws:logs:*:*:*"]
  }
}

data "aws_iam_policy_document" "allow_s3" {
  statement {
    effect = "Allow"
    actions = [
      "s3:PutObject",
      "s3:GetObject",
      "s3:ListBucket",
      "s3:DeleteObject"
    ]
    resources = [
      "arn:aws:s3:::*", # this should be more specific once it is created: /recipedata
    ]
  }
}

# Create an IAM Role for the Lambda. Its rules are defined in the Policy Document.
resource "aws_iam_role" "lambda_iam_role" {
  name               = "iam_for_lambda"
  assume_role_policy = data.aws_iam_policy_document.lambda_assume_role_policy.json
}

# Create an IAM Policy for Logging & S3. Its rules are defined in the Policy Document.
resource "aws_iam_policy" "lambda_logging" {
  name   = "lambda-logging-cloudwatch"
  policy = data.aws_iam_policy_document.lambda_allow_logging.json
}

resource "aws_iam_policy" "lambda_s3" {
  name   = "lambda-s3-policy"
  policy = data.aws_iam_policy_document.allow_s3.json
}

# Attach the policies to the IAM Role

resource "aws_iam_role_policy_attachment" "lambda_logging_policy_attachment" {
  role       = aws_iam_role.lambda_iam_role.id
  policy_arn = aws_iam_policy.lambda_logging.arn
}

resource "aws_iam_role_policy_attachment" "lambda_s3_policy_attachment" {
  role       = aws_iam_role.lambda_iam_role.id
  policy_arn = aws_iam_policy.lambda_s3.arn
}