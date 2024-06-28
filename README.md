# Expectativa_do_Mercado_Mensal📊
## Introdução e Objetivo do projeto💡
O objetivo deste projeto é buscar, através da **API do BACEN **(https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/swagger-ui3#/), dados da série histórica da expectativa do mercado mensal para um período selecionável e para um tipo de indicador (IPCA, IGP-M ou Selic). Esses dados são apresentados em um Data Grid, e, a partir deles, é implementado um gráfico de linhas que representa a expectativa mensal da Mediana dos próximos 3 meses, observando as expectativas anteriores, semelhante ao **Focus-Relatório de Mercado**(https://www.bcb.gov.br/controleinflacao/relatoriofocus). Também será possível salvar os dados no banco de dados SQL Server (criado a partir de um container), além de exportar os dados para CSV.
## Ferramentas Utilizadas🛠️

### .NET 6

- **Framework:** .NET 6 é a plataforma de desenvolvimento utilizada para criar a aplicação.
- **Recursos:** Compatibilidade com várias plataformas, melhorias de desempenho, novos recursos de linguagem e bibliotecas de classe.

### WPF (Windows Presentation Foundation)

- **Framework:** Utilizado para desenvolver a interface gráfica da aplicação.
- **Recursos:** Suporte a XAML para definição de interface, vinculação de dados, animações e gráficos vetoriais.

### SQL Server

- **Banco de Dados:** SQL Server é utilizado como o sistema de gerenciamento de banco de dados.
- **Recursos:** Suporte a T-SQL, alta performance, segurança e escalabilidade.

### Docker

- **Containerização:** Docker é utilizado para criar e gerenciar contêineres, facilitando a configuração do ambiente de desenvolvimento e a implantação da aplicação.
- **Recursos:** Portabilidade de aplicativos, isolamento de processos, gerenciamento de dependências.

## Configuração do Ambiente⚙️

### Pré-requisitos

- [Docker](https://www.docker.com/get-started)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)(Opcional, já que estamos subindo em um Docker, será interessante instalar apenas para verificar se os dados estão sendo armazenados corretamente)
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio](https://visualstudio.microsoft.com/) (opcional, mas recomendado para desenvolvimento WPF)
### Importante comentar...
Para que a interação com o banco de dados seja possível, é necessário instalar o container criado no arquivo ```docker-compose.yml```. Para isso, acesse a pasta do projeto pelo terminal e execute o seguinte comando:
```bash
docker-compose up
````
Após fechar a aplicação, é importante executar outro comando para encerrar o container:
```bash
docker-compose down
````
## Arquitetura Utilizada 🏛️
Foi utilizada a arquitetura MVVM, onde os componentes View interagem com os ViewModel e os ViewModel interagem com os Models. Além disso, foi criada uma pasta de Services para a integração com o banco de dados e para acessar a API.
Leia mais sobre: MVVM.
## Regras Aplicadas 📜
- O usuario pode escolher apenas um indicador(**IPCA,IGP-M,Selic**).
- Deve-se escolher a Data Inicio e a Data fim. Foi Travada datas limites para ambas, tanto data minima quanto data máxima.Existe uma regra de ser necessário a data inicial ser antes da data final.
- Foi criado um tratamento de erro caso a API esteja fora do ar.
- A **Selic**é consumida por um método GET diferente, tendo sua modelagem distinta de IPCA e IGP-M. A expectativa da Selic é dada pela data da reunião do COPOM, enquanto as demais são pelo mês de referência. Para evitar duplicação de código, adaptei a taxa Selic para a modelagem das demais, sendo a data de referência as reuniões.
-  Por esse efeito  foi necessário um tratamento diferente dos dados para o gráfico já que ocorrem apenas 8 Reuniões no ano. Foi realizado um cálculo matemático para verificar em quais meses a reunião atende para que possa plotar o gráfico  de forma coesa.
- Os gráficos de IPCA e IGP-M possuem 3 linhas representando os próximos meses de expectativa a partir da data final. Já o de Selic possui 2 linhas representando as próximas reuniões: uma para um período de dois meses e outra para um período de um mês.
- É possível exportar os dados para um CSV. Ao pressionar o botão, o arquivo será adicionado na pasta Downloads da sua máquina, versionando a cada exportação.
- Caso tenha rodado o Docker, é possível salvar esses dados em um banco de dados SQL Server. Não foram tratadas verificações de duplicidade de dados, sendo a única chave diferenciadora o ID (incremental)..
- O código não está implementado tela de loading, portanto pode ser que dependendo da ação feita, a tela da aplicação trave, mas não se preocupe ele está apenas executando a tarefa passada. Caso não seja retornado com sucesso mostrará o erro.
- Não foi criado o arquivo.ENV para as variaveis de ambiente, como a configuração se da pelo docker com o mesmo ambiente para todos os usuarios que rodarem o projeto, defini variaveis padrões para todos no ```App.settings```
