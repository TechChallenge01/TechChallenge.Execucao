resource "aws_ecr_repository" "execucao_service" {
  name                 = "${var.project_name}-execucao-service"
  image_tag_mutability = "MUTABLE"

  image_scanning_configuration {
    scan_on_push = true
  }

  tags = { Name = "execucao-service" }
}

resource "aws_ecr_lifecycle_policy" "execucao_service" {
  repository = aws_ecr_repository.execucao_service.name

  policy = jsonencode({
    rules = [{
      rulePriority = 1
      description  = "Manter apenas as últimas 5 imagens"
      selection = {
        tagStatus   = "any"
        countType   = "imageCountMoreThan"
        countNumber = 5
      }
      action = { type = "expire" }
    }]
  })
}
