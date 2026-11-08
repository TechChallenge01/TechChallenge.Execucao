resource "aws_cloudwatch_log_group" "execucao_service" {
  name              = "/ecs/execucao-service"
  retention_in_days = 7

  tags = { Name = "execucao-service-logs" }
}
