variable "lambda_name" {
  type    = string
  default = ""
}

variable "runtime" {
  type    = string
  default = "dotnet8"
}

variable "memory_size" {
  type    = string
  default = 512
}

variable "timeout" {
  type    = string
  default = 15
}

variable "project_path" {
  type = string
}