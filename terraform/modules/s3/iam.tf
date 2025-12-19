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

resource "aws_iam_policy" "recipe_platform_s3" {
  name   = var.s3_policy_name
  policy = data.aws_iam_policy_document.allow_s3.json
}