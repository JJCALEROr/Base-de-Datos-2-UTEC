USE GestorInventario;
GO

-- ============================================================
-- VISTA 1: Productos con stock bajo el mínimo
-- ============================================================
CREATE VIEW vw_ProductosBajoStock AS
SELECT
    p.id_producto,
    p.nombre_producto,
    c.nombre_categoria,
    pr.nombre_proveedor,
    m.unidad_medida,
    p.stock_actual,
    p.stock_minimo,
    (p.stock_minimo - p.stock_actual) AS unidades_faltantes,
    p.precio_compra,
    p.precio_venta,
    p.estado
FROM Producto p
INNER JOIN Categoria       c  ON p.id_categoria = c.id_categoria
INNER JOIN Proveedor       pr ON p.id_proveedor  = pr.id_proveedor
INNER JOIN Medidas         m  ON p.id_medida     = m.id_medida
WHERE p.stock_actual < p.stock_minimo
  AND p.estado = 'Activo';
GO

-- ============================================================
-- VISTA 2: Resumen de ventas por mes y año
-- ============================================================
CREATE VIEW vw_VentasPorMes AS
SELECT
    YEAR(v.fecha_venta)             AS anio,
    MONTH(v.fecha_venta)            AS mes,
    DATENAME(MONTH, v.fecha_venta)  AS nombre_mes,
    COUNT(DISTINCT v.id_venta)      AS total_ventas,
    SUM(dv.cantidad)                AS unidades_vendidas,
    SUM(dv.subtotal)                AS ingresos_brutos,
    AVG(v.total_venta)              AS ticket_promedio
FROM Venta v
INNER JOIN DetalleVenta dv ON v.id_venta = dv.id_venta
WHERE v.estado = 'Completada'
GROUP BY
    YEAR(v.fecha_venta),
    MONTH(v.fecha_venta),
    DATENAME(MONTH, v.fecha_venta);
GO

-- ============================================================
-- VISTA 3: Productos más vendidos (cantidad y monto)
-- ============================================================
CREATE VIEW vw_ProductosMasVendidos AS
SELECT
    p.id_producto,
    p.nombre_producto,
    c.nombre_categoria,
    SUM(dv.cantidad)                        AS total_unidades_vendidas,
    SUM(dv.subtotal)                        AS total_ingresos,
    COUNT(DISTINCT dv.id_venta)             AS numero_ventas,
    AVG(dv.precio_unitario)                 AS precio_promedio_venta,
    RANK() OVER (ORDER BY SUM(dv.cantidad) DESC) AS ranking
FROM DetalleVenta dv
INNER JOIN Venta    v  ON dv.id_venta    = v.id_venta
INNER JOIN Producto p  ON dv.id_producto = p.id_producto
INNER JOIN Categoria c ON p.id_categoria = c.id_categoria
WHERE v.estado = 'Completada'
GROUP BY
    p.id_producto,
    p.nombre_producto,
    c.nombre_categoria;
GO

-- ============================================================
-- VISTA 4: Rentabilidad por producto
-- ============================================================
CREATE VIEW vw_RentabilidadProducto AS
SELECT
    p.id_producto,
    p.nombre_producto,
    c.nombre_categoria,
    p.precio_compra,
    p.precio_venta,
    (p.precio_venta - p.precio_compra)                          AS margen_unitario,
    CAST(
        ((p.precio_venta - p.precio_compra) / p.precio_compra)
        * 100 AS DECIMAL(10,2))                                 AS porcentaje_margen,
    SUM(dv.cantidad)                                            AS unidades_vendidas,
    SUM(dv.cantidad * (p.precio_venta - p.precio_compra))       AS ganancia_total
FROM Producto p
INNER JOIN Categoria   c  ON p.id_categoria = c.id_categoria
LEFT  JOIN DetalleVenta dv ON p.id_producto  = dv.id_producto
LEFT  JOIN Venta        v  ON dv.id_venta    = v.id_venta
                           AND v.estado = 'Completada'
GROUP BY
    p.id_producto,
    p.nombre_producto,
    c.nombre_categoria,
    p.precio_compra,
    p.precio_venta;
GO

-- ============================================================
-- VISTA 5: Compras por proveedor y período
-- ============================================================
CREATE VIEW vw_ComprasPorProveedor AS
SELECT
    pr.id_proveedor,
    pr.nombre_proveedor,
    pr.empresa,
    YEAR(c.fecha_compra)            AS anio,
    MONTH(c.fecha_compra)           AS mes,
    DATENAME(MONTH, c.fecha_compra) AS nombre_mes,
    COUNT(DISTINCT c.id_compra)     AS numero_ordenes,
    SUM(dc.cantidad)                AS unidades_compradas,
    SUM(dc.subtotal)                AS monto_total_comprado
FROM Proveedor pr
INNER JOIN Producto     p  ON pr.id_proveedor = p.id_proveedor
INNER JOIN DetalleCompra dc ON p.id_producto  = dc.id_producto
INNER JOIN Compra        c  ON dc.id_compra   = c.id_compra
WHERE c.estado = 'Completada'
GROUP BY
    pr.id_proveedor,
    pr.nombre_proveedor,
    pr.empresa,
    YEAR(c.fecha_compra),
    MONTH(c.fecha_compra),
    DATENAME(MONTH, c.fecha_compra);
GO

-- ============================================================
-- VISTA 6: Movimientos de inventario con detalle
-- ============================================================
CREATE VIEW vw_MovimientosInventario AS
SELECT
    mi.id_movimiento,
    p.nombre_producto,
    c.nombre_categoria,
    mi.tipo_movimiento,
    mi.cantidad,
    mi.fecha_movimiento,
    mm.motivo,
    mi.comentario,
    -- stock antes/después es aproximado: útil para auditoría
    SUM(
        CASE mi.tipo_movimiento
            WHEN 'Entrada' THEN  mi.cantidad
            WHEN 'Salida'  THEN -mi.cantidad
        END
    ) OVER (
        PARTITION BY mi.id_producto
        ORDER BY mi.fecha_movimiento, mi.id_movimiento
        ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
    ) AS stock_acumulado
FROM MovimientoInventario mi
INNER JOIN Producto          p  ON mi.id_producto = p.id_producto
INNER JOIN Categoria         c  ON p.id_categoria = c.id_categoria
INNER JOIN MotivoMovimiento  mm ON mi.id_motivo   = mm.id_motivo;
GO

-- ============================================================
-- VISTA 7: Productos próximos a caducar (90 días)
-- ============================================================
CREATE VIEW vw_ProductosProximosCaducar AS
SELECT
    p.id_producto,
    p.nombre_producto,
    c.nombre_categoria,
    pr.nombre_proveedor,
    p.fecha_caducidad,
    DATEDIFF(DAY, CAST(GETDATE() AS DATE), p.fecha_caducidad) AS dias_restantes,
    p.stock_actual,
    p.stock_actual * p.precio_venta                           AS valor_en_riesgo,
    CASE
        WHEN DATEDIFF(DAY, CAST(GETDATE() AS DATE), p.fecha_caducidad) <= 15  THEN 'Crítico'
        WHEN DATEDIFF(DAY, CAST(GETDATE() AS DATE), p.fecha_caducidad) <= 30  THEN 'Urgente'
        ELSE 'Próximo'
    END AS nivel_alerta
FROM Producto p
INNER JOIN Categoria c  ON p.id_categoria = c.id_categoria
INNER JOIN Proveedor pr ON p.id_proveedor  = pr.id_proveedor
WHERE p.fecha_caducidad IS NOT NULL
  AND p.fecha_caducidad <= DATEADD(DAY, 90, CAST(GETDATE() AS DATE))
  AND p.fecha_caducidad >= CAST(GETDATE() AS DATE)
  AND p.estado = 'Activo';
GO

-- ============================================================
-- VISTA 8: Actividad de usuarios (ventas y compras registradas)
-- ============================================================
CREATE VIEW vw_ActividadUsuarios AS
SELECT
    u.id_usuario,
    u.nombre + ' ' + u.apellido     AS nombre_completo,
    u.correo,
    r.nombre_rol,
    COUNT(DISTINCT v.id_venta)       AS ventas_registradas,
    ISNULL(SUM(v.total_venta), 0)    AS monto_ventas,
    COUNT(DISTINCT c.id_compra)      AS compras_registradas,
    ISNULL(SUM(c.total_compra), 0)   AS monto_compras,
    u.estado
FROM Usuario u
INNER JOIN Roles          r ON u.id_rol    = r.id_rol
LEFT  JOIN Venta          v ON u.id_usuario = v.id_usuario AND v.estado  = 'Completada'
LEFT  JOIN Compra         c ON u.id_usuario = c.id_usuario AND c.estado  = 'Completada'
GROUP BY
    u.id_usuario,
    u.nombre,
    u.apellido,
    u.correo,
    r.nombre_rol,
    u.estado;
GO

-- ============================================================
-- VISTA 9: Valorización del inventario por categoría
-- ============================================================
CREATE VIEW vw_ValorizacionInventario AS
SELECT
    c.nombre_categoria,
    COUNT(p.id_producto)                        AS total_productos,
    SUM(p.stock_actual)                         AS unidades_totales,
    SUM(p.stock_actual * p.precio_compra)       AS valor_costo,
    SUM(p.stock_actual * p.precio_venta)        AS valor_venta,
    SUM(p.stock_actual * (p.precio_venta - p.precio_compra)) AS ganancia_potencial,
    CAST(
        SUM(p.stock_actual * p.precio_compra) * 100.0
        / NULLIF(SUM(SUM(p.stock_actual * p.precio_compra)) OVER (), 0)
    AS DECIMAL(10,2))                           AS porcentaje_del_total
FROM Categoria c
INNER JOIN Producto p ON c.id_categoria = p.id_categoria
WHERE p.estado = 'Activo'
GROUP BY c.nombre_categoria;
GO

-- ============================================================
-- VISTA 10: Comparativo compras vs ventas por producto
-- ============================================================
CREATE VIEW vw_ComparativoCompraVenta AS
SELECT
    p.id_producto,
    p.nombre_producto,
    c.nombre_categoria,
    ISNULL(SUM(dc.cantidad), 0)                     AS unidades_compradas,
    ISNULL(SUM(dc.subtotal), 0)                     AS costo_total_comprado,
    ISNULL(SUM(dv.cantidad), 0)                     AS unidades_vendidas,
    ISNULL(SUM(dv.subtotal), 0)                     AS ingreso_total_vendido,
    ISNULL(SUM(dv.subtotal), 0)
        - ISNULL(SUM(dc.subtotal), 0)               AS balance,
    ISNULL(SUM(dc.cantidad), 0)
        - ISNULL(SUM(dv.cantidad), 0)               AS stock_neto_movido
FROM Producto p
INNER JOIN Categoria      c   ON p.id_categoria = c.id_categoria
LEFT  JOIN DetalleCompra  dc  ON p.id_producto  = dc.id_producto
LEFT  JOIN DetalleVenta   dv  ON p.id_producto  = dv.id_producto
LEFT  JOIN Compra         co  ON dc.id_compra   = co.id_compra  AND co.estado = 'Completada'
LEFT  JOIN Venta          v   ON dv.id_venta    = v.id_venta    AND v.estado  = 'Completada'
GROUP BY
    p.id_producto,
    p.nombre_producto,
    c.nombre_categoria;
GO