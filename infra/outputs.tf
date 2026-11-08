output "execucao_service_url" {
  description = "URL pública do Execucao Service"
  value       = "http://${aws_lb.execucao_service.dns_name}"
}

output "ecr_execucao_repository_url" {
  description = "URL do repositório ECR do Execucao Service"
  value       = aws_ecr_repository.execucao_service.repository_url
}
