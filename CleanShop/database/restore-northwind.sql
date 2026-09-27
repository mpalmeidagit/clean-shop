-- Restaura o Northwind a partir do backup na primeira subida do container.
-- Idempotente: se o banco já existe no volume, não faz nada.
IF DB_ID('Northwind') IS NULL
BEGIN
    PRINT 'Restaurando Northwind a partir de /var/opt/mssql/backup/Northwind.bak...';

    RESTORE DATABASE Northwind
    FROM DISK = '/var/opt/mssql/backup/Northwind.bak'
    WITH MOVE 'Northwind' TO '/var/opt/mssql/data/Northwind.mdf',
         MOVE 'Northwind_log' TO '/var/opt/mssql/data/Northwind_log.ldf';
END
ELSE
    PRINT 'Northwind já existe. Restauração ignorada.';
