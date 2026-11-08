# TechChallenge.Execucao — Microsserviço de Execução

Microsserviço responsável pelo controle da execução do reparo, parte do sistema TechChallenger (oficina mecânica).

## Responsabilidades

- Controle de etapas de execução: Recebida → EmPreparacao → Pronta → Entregue
- Registro do valor final do serviço
- Publicação de evento de conclusão para o OS Service
- Tratamento e registro de falhas na execução

## Arquitetura

Clean Architecture com 5 camadas: **Domain**, **Shared**, **Application**, **Infra**, **API**.

## Banco de Dados

- **MongoDB** — banco de dados NoSQL
- Coleção: `execucoes`
- Cada documento armazena a execução com lista de etapas embutida (Document Model)

## Padrão Saga — Coreografado via RabbitMQ

### Por que Coreografia?

Ao receber `pagamento.confirmado`, o Execução inicia o reparo automaticamente sem ser chamado diretamente pelo OS Service. Ao finalizar, publica `execucao.finalizada` para que o OS Service atualize seu status. Isso mantém o desacoplamento total.

### Eventos publicados

| Exchange | Routing Key | Quando |
|----------|-------------|--------|
| `tech.saga` | `execucao.iniciada` | Execução criada após pagamento confirmado |
| `tech.saga` | `execucao.finalizada` | Reparo concluído com valor final |
| `tech.saga` | `execucao.falhou` | Falha durante a execução do reparo |

### Eventos consumidos

| Exchange | Routing Key | Ação |
|----------|-------------|------|
| `tech.saga` | `pagamento.confirmado` | Cria ExecucaoPedido e inicia reparo |

### Compensação (Rollback)

- Falha na execução → publica `execucao.falhou` com motivo
- OS Service aguarda retry manual ou intervenção
- Cada falha é registrada como etapa com motivo na coleção
- Dead Letter Queue (`execucao.saga.dlq`)

## Executando localmente

```bash
# Pré-requisitos: Docker, .NET 10 SDK
docker compose up -d
dotnet run --project src/API
```

## Variáveis de ambiente

| Variável | Descrição |
|----------|-----------|
| `ConnectionStrings__MongoDB` | URI do MongoDB |
| `MongoDB__DatabaseName` | Nome do banco (default: `techchallenge_execucao`) |
| `RabbitMQ__Host` | Host do RabbitMQ |
| `RabbitMQ__Username` | Usuário RabbitMQ |
| `RabbitMQ__Password` | Senha RabbitMQ |
| `Jwt__Key` | Chave JWT |

## Endpoints principais

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/execucoes/{id}` | Buscar execução por ID |
| `GET` | `/execucoes/pedido/{pedidoId}` | Buscar execução por pedido/OS |
| `POST` | `/execucoes/{id}/preparar` | Marcar em preparação |
| `POST` | `/execucoes/{id}/pronto` | Marcar como pronto |
| `POST` | `/execucoes/{id}/finalizar` | Finalizar com valor final |

## Kubernetes

Manifestos em `k8s/`: `deployment.yaml`, `service.yaml`, `hpa.yaml`.

## CI/CD

| Workflow | Trigger | Descrição |
|----------|---------|-----------|
| `deploy.yml` | push main | Build, ECR, ECS deploy |
| `infra.yml` | push main (infra/) | Terraform apply |
| `quality.yml` | push/PR main | Testes + SonarCloud |
| `k8s-deploy.yml` | manual | Deploy para EKS |

## Testes

```bash
dotnet test tests/UnitTests
```

Cobertura mínima: **80%** — validada pelo SonarCloud.

**Configuração SonarCloud**: Secret `SONAR_TOKEN`, Variables `SONAR_PROJECT_KEY` e `SONAR_ORGANIZATION`.
