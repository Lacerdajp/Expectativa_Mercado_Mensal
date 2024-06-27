# Expectativa_do_Mercado_Mensal📊
## Introdução e Objetivo do projeto💡
O projeto traz como objetivo buscar pela **API do BACEN** (https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/swagger-ui3#/) 
 os dados da serie histórica da expctativa do mercado mensal de um periodo selecionavel e de um tipo de indicador(IPCA,IGP-M ou Selic). Esses dados são apresentados em um Data Grid, e a partir desses dados é implementado um gráfico de linhas representado a expectativa mensal da Mediana dos proximos 3 meses observando as expectativas anteriores, assim como a **Focus-Relatorio de mercado** (https://www.bcb.gov.br/controleinflacao/relatoriofocus).Será possivel também salvar no Banco de dados em SQL Server(criado a partir de um Container), além disso será possivel exportar os dados para CSV. 

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
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)(Opcional como estamos subindo em um Docker, será interesssante instalar apenas para checar se realmente os dados estão sendo armazenados)
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio](https://visualstudio.microsoft.com/) (opcional, mas recomendado para desenvolvimento WPF)
### Importante comentar...
Para que a iteração com o banco de dados seja possivel é necessario instalar o containner criado no arquivo ```docker-compose.yml```. Para  isso acesse a pasta do projeto pelo terminal e execute o seguite comando:
```bash
docker-compose up
````
Após fechar a aplicação é muito importante executar outro comando para encerrar o container:
```bash
docker-compose down
````
## Arquitetura Utilizada 🏛️
Foi utilizado a arquitetura MVVM, Onde  os componentes View interagem com os VM e os Vm interagem com os Models, além disso foi criada uma pasta de services para criar a integração com o Banco de Dados e para acessar a API. 
Leia mais sobre: https://learn.microsoft.com/pt-br/windows/uwp/data-binding/data-binding-and-mvvm
## Regras Aplicadas 📜
- O usuario pode escolher apenas um indicador(**IPCA,IGP-M,Selic**).
- Deve-se escolher a Data Inicio e a Data fim. Foi Travada datas limites para ambas, tanto data minima quanto data máxima.Existe uma regra de ser necessário a data inicial ser antes da data final.
- Foi criado um tratamento de erro caso a API seja quebrada.
- **Selic** era consumida por outro método Get, tendo sua modelagem diferente de IPCA e IGM-P, sua unica diferença é que a expectativa é dada pela data da reunião da COPOM, e os demais outra pelo mês de referência. Para que não precisasse duplicar códigos adaptei a taxa Selic para a modelagem das demais, sendo a data de referencia as reuniões.
- Por esse efeito  foi necessário um tratamento diferente dos dados para o gráfico já que ocorrem apenas 8 Reuniões no ano. Foi realizado um cálculo matemático para verificar em quais meses a reunião atende para que possa plotar o gráfico  de forma coesa.
- Com isso os gráficos de IPCA e IGP-M possuem 3 linhas que são os próximos meses de expectativa a partir da data final.Já o de Selic as linhas representam as 2 Reuniões, uma pra um periodo de dois meses e outra para um perido de 1 mês.
- É possivel exportar os dados para um CSV, ao apertar o botão será adiciondo na pasta Downloads da sua Máquina, versionando cada vez que você exportar.
- Caso tenha rodado o Docker é possivel salvar esses dados em um banco de Dados SQL Server, não foram tratadas as verificaçõs se os arquivos importados no banco são repetidos, sendo asssim a unica Key que diferencia os dados é o ID(incremental).



