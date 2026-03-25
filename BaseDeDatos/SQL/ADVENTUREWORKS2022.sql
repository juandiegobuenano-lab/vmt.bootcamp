Use [AdventureWorksLT2022]
GO


---DISTINCT: Para encontrar datos unicos de la columna
-- Top:Da exactamenten el numero de datos q deseas.
SELECT distinct top 2  Color as Producto_Color from SalesLT.Product
where Color is not null;

Select distinct count(*) from SalesLT.SalesOrderHeader



DECLARE @PAGE_NUMBER INT = 1;
DECLARE @ROWS_PER_PAGE INT = 6;

SELECT distinct Color as Producto_Color from SalesLT.Product
where Color is not null
ORDER BY Color
OFFSET (@PAGE_NUMBER - 1) * @ROWS_PER_PAGE ROWS--EMPIEZA DESDE ESTA FILA
FETCH NEXT @ROWS_PER_PAGE rows ONLY; --hasta la fila 



SELECT distinct Color as Producto_Color from SalesLT.Product
where Color is not null
ORDER BY Color
OFFSET 2 ROWS --EMPIEZA DESDE ESTA FILA
FETCH NEXT 5  rows ONLY; --hasta la fila 

--order by:
SELECT * from SalesLT.ProductCategory
ORDER BY Name DESC

Select * from SalesLT.SalesOrderHeader
order by AccountNumber


--LIKE: 
select * from SalesLT.Customer
where FirstName LIKE '%Rob%';

--BETWEEN:

select FirstName, LastName, ModifiedDate from SalesLT.Customer
where ModifiedDate between '2005' AND '2009';

-- COUNT:
Select COUNT(*) as TOTAL_CUSTOMERS FROM SalesLT.Customer

-- SUM:
Select SUM(TotalDue) as TOTAL_VENTAS FROM SalesLT.SalesOrderHeader

--GROUP BY:
SELECT COUNT(*) FROM SalesLT.Product


Select ProductCategoryID AS CATEGORIA_ID, COUNT(ProductCategoryID) as Productos_TOTALES  from SalesLT.Product
GROUP BY  ProductCategoryID
ORDER BY Productos_TOTALES DESC

--INNER JOIN
SELECT * FROM SalesLT.Product p
inner join SalesLT.ProductCategory pc
on p.ProductCategoryID=pc.ProductCategoryID

SELECT * FROM SalesLT.SalesOrderHeader SOH
inner join SalesLT.Customer C
on SOH.CustomerID = C.CustomerID


-- SUM: SUMA TOTAL, AVG: PROMEDIO, MIN/MAX. VALORES EXTREMOS, COUNT(*): CUANTAS FILAS, COUNT(col): Cuenta todo menos NULL.
-- WHERE: FILTRA FILAS, HAVING: FILTRA GRUPOS.

--BD: [AdventureWorksLT2022]
-- 1.- TOTAL DE CLIENTES
Select COUNT(*) as TOTAL_CLIENTES FROM SalesLT.Customer

--2.- TOTAL DE VENTAS EN EL MES DE OCTUBRE

DECLARE @FECHA DATETIME2 = CONVERT(DATETIME2, '2007-10-28',23);
Select SUM(TotalDue) as TOTAL_VENTAS_OCTUBRE FROM SalesLT.SalesOrderHeader
where MONTH(OrderDate)=10;




-- 3.- ORDENEN LAS CATEGORIAS POR NOMBRES.
Select name from SalesLT.ProductCategory
Order by Name ASC;

--4.- QUE RELACIONEN CABECERA Y DETALLE FACTURA.
SELECT 
	SOH.SalesOrderID,
	SOH.OrderDate,
	SOD.OrderQty,
	SOD.LineTotal
FROM SalesLT.SalesOrderHeader SOH
INNER JOIN SalesLT.SalesOrderDetail SOD
ON SOH.SalesOrderID=SOD.SalesOrderID;

--5.- IMPLEMENTACION PAGINACION
SELECT * FROM SalesLT.Customer
ORDER BY CustomerID
OFFSET 0 ROWS
FETCH NEXT 10 ROWS ONLY;


--6.- USO DE DISTINCT Y TOP
SELECT distinct top 5  CompanyName as Nombre_Empresa from SalesLT.Customer
where CompanyName is not null;



