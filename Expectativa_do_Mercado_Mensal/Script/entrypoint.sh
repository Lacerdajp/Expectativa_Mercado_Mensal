#!/bin/bash

# Aguardar o SQL Server iniciar
sleep 15s

# Executar o script SQL
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P expcMercad2024 -d master -i /script/setup.sql
