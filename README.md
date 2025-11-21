# Notes

## RecipePlatform.DataManager
- Eventually we will need to create a Function URL to make the endpoints accessible

## RecipePlatform.Core
- This will hold any shared models

## Terraform Refactor

Right now the following variables are defined:

```terraform
variable "cloudwatch_policy_name" {
  type = string
}

variable "s3_policy_name" {
  type = string
}

variable "dynamodb_policy_name" {
  type = string
}
```

This prescribes the lambda resources defined in `main.tf` to define a value for each, even if they don't use
the resource. In the future, this needs refactoring. Each lambda should define the resources it wants to use
in the `main.tf` file and then pass a collection of `aws_iam_policy_document` instances to the module. The module
can then respond dynamically and set whatever resources are passed down.~~~~
