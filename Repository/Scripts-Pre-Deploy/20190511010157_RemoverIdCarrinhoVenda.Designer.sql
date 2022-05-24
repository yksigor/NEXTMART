USE NEXTMART;

DELETE FROM VENDAS WHERE IdCarrinho is not null or IdUsuario is null;