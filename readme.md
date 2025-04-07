## **Propósito do Projeto**
O projeto tem por objetivo simular o processo de reservas de assentos por trecho em uma viagem.

## **Entidades** 

### **Department**
       
Representa uma ponto de uma parada na rota que será percorrerida na viagem.

### **Ticket**

Representa a compra de trecho do assento da viagem, sendo composto pela a origem e o destino da viagem, bem como a viagem e o assento que foi reservado.

### Travel

Representa a viagem, contendo uma lista de assentos, uma lista de bilhetes, a data e hora da viagem, a rota de departamentos e a capacidade máxima de assentos.

#### Funcionaldiades e regras de negócio:
1. **Viagem:**
   - Uma viagem deve ter pelo menos um departamento na rota.
   - O número de assentos da lista não pode ultrapassar o valor máximo de ```MaxSeatsCount```.

2. **Reservar um ticket na viagem:**
   - Para reservar um ticket, o assento deve estar disponível no trecho da viagem.
   - Se o assento ainda não existir na viagem, ele deve ser adicionado.
   - Por fim o assento deve ser marcado como ocupado da origem ao destino.

3. **Verificar assentos disponíveis:**
   - Um assento estará disponível se o bitmap estiver com os bits zerados para o trecho a ser verificado.
   - A função `GetAllAvailableSeats(startDeptId, endDeptId)` deve retornar todos os assentos livres para o determinado trecho passado.

4. **Lista de Assentos:**
   - Um assento não pode ser adicionado se a capacidade da viagem estiver cheia.
   - Não pode haver dois assentos com o mesmo número na mesma viagem.

5. **Lista de Bilhetes:**
   - Um bilhete está atrelado a um assento e a origem e destino da viagem.
   - Antes de adicionar o bilhete, o assento deve estar disponível para a reserva.

6. **Verificação de Capacidade:**
   - Uma viagem está totalmente cheia quando todos os assentos estão ocupados.

### TravelSeat
Representa um assento dentro de uma viagem, composto pelo bitmap que é usado para marcar os trechos ocupados.

#### Regras:
1. **Verificação de Disponibilidade:**
   - O assento estará disponível para um determinado trecho se no bitmap os bit estiver marcado com 0 da origem até o destino.
   - O assento estará disponível se houver ao menos um trecho no bitmap marcodo com 0.

2. **Atualização do Bitmap:**
   - Quando o assento é reservado, os bits correspondentes ao trecho são marcados com o valor 1.
   - Se o trecho estiver ocupado, deve cancelar a atualização do bitmap.

3. **Assento:**
   - Um assento deve estar atrelado a uma viagem e deve ter um número do assento exclusivo dentro dessa viagem.

## **Como executar**

1. Clone o repositório:
   ```bash
   git clone https://github.com/Guilherme-DSGL/PyramidTests.git
   cd PyramidTests
   ```
2. Instale as dependências:
   ```bash
   dotnet restore
   ```
3. Rode o projeto: 
   ```bash
   cd ConsoleApp
   dotnet run
   ```

## **Como rodar os testes automatizados**
   
   Para rodar os testes automatizados, basta executar o comando abaixo dentro de Pyramid.Tests

   ```bash 
   dotnet test 
   ```

   Para executar com a cobertura do coverlet, basta executar os comandos abaixo

   ```bash
   cd Pyramid.Tests
   dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./coverage/ 
   ```

   E em seguida para gerar uma vizualiação html dos dados gerados basta executar o comando abaixo

   ```bash
   reportgenerator -reports:"coverage/coverage.opencover.xml" -targetdir:"TestResults/CoverageReport" -reporttypes:Html
   ```

   Na pasta gerada ```CoverageReport``` basta abrir o arquivo ```index.html``` com o navegador de sua escolha.

   