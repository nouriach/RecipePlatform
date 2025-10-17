data "aws_region" "current" {}
data "aws_caller_identity" "current" {}

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
      "arn:aws:s3:::*",
    ]
  }
}

data "aws_iam_policy_document" "allow_dynamodb" {
  statement {
    effect = "Allow"
    actions = [
      "dynamodb:GetItem",
      "dynamodb:BatchGetItem",
      "dynamodb:Query",
      "dynamodb:Scan",
      "dynamodb:UpdateItem",
      "dynamodb:BatchWriteItem"
    ]
    resources = [
      "arn:aws:dynamodb:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:table/Recipes",
      "arn:aws:dynamodb:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:table/Recipes/index/*"
    ]
  }
}

resource "aws_iam_role" "recipeplatform_iam_role" {
  name               = "recipeplatform_for_lambda"
  assume_role_policy = data.aws_iam_policy_document.lambda_assume_role_policy.json
}

resource "aws_iam_policy" "recipeplatform_lambda_logging" {
  name   = "recipeplatform-logging-cloudwatch"
  policy = data.aws_iam_policy_document.lambda_allow_logging.json
}

resource "aws_iam_policy" "recipeplatform_lambda_s3" {
  name   = "recipeplatform-s3-policy"
  policy = data.aws_iam_policy_document.allow_s3.json
}

resource "aws_iam_role_policy" "recipeplatform_lambda_dynamodb" {
  name   = "recipeplatform-dynamodb-policy"
  role   = aws_iam_role.recipeplatform_iam_role.id
  policy = data.aws_iam_policy_document.allow_dynamodb.json
}

resource "aws_iam_role_policy_attachment" "lambda_logging_policy_attachment" {
  role       = aws_iam_role.recipeplatform_iam_role.id
  policy_arn = aws_iam_policy.recipeplatform_lambda_logging.arn
}

resource "aws_iam_role_policy_attachment" "lambda_s3_policy_attachment" {
  role       = aws_iam_role.recipeplatform_iam_role.id
  policy_arn = aws_iam_policy.recipeplatform_lambda_s3.arn
}