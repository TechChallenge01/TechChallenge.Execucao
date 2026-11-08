resource "aws_secretsmanager_secret" "execucao_service" {
  name                    = "${var.project_name}/execucao-service"
  recovery_window_in_days = 0

  tags = { Name = "execucao-service-secrets" }
}

resource "aws_secretsmanager_secret_version" "execucao_service" {
  secret_id = aws_secretsmanager_secret.execucao_service.id

  secret_string = jsonencode({
    "MongoDB__ConnectionString" = "mongodb://admin:${var.mongodb_root_password}@${data.aws_instance.services.private_ip}:27017/?authSource=admin"
    "RabbitMQ__Host"            = data.aws_instance.services.private_ip
    "RabbitMQ__UserName"        = var.rabbitmq_username
    "RabbitMQ__Password"        = var.rabbitmq_password
    "Jwt__Key"                  = var.jwt_key
  })
}
