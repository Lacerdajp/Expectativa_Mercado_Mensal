#!/bin/bash

# Aguardar o SQL Server iniciar
sleep 15s

# Executar o script SQL
#result=$(/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P expcMercad2024 -d master -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM sys.databases WHERE name = 'ExpectativaMercado';" | tr -d '[:space:]')
#if [ "$result" = 0 ]; then
	#echo "O banco de dados ainda nao existe... estamos criando"
	/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P expcMercad2024 -d master -i /script/setup.sql
#else 
	#echo "O banco de dados já existe, basta executar a aplicação"
	#fi
