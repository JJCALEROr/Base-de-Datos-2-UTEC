USE GestorInventario;
GO

-- ============================================================
--  CATEGORIA
-- ============================================================
INSERT INTO Categoria (nombre_categoria, descripcion) VALUES
('Electrónica',     'Dispositivos y accesorios electrónicos'),
('Alimentos',       'Productos alimenticios y bebidas'),
('Limpieza',        'Artículos de limpieza e higiene'),
('Papelería',       'Útiles de oficina y escolares'),
('Herramientas',    'Herramientas manuales y eléctricas'),
('Ropa',            'Prendas de vestir y accesorios'),
('Medicamentos',    'Productos farmacéuticos de venta libre'),
('Juguetes',        'Juguetes y artículos de entretenimiento'),
('Deportes',        'Equipos y accesorios deportivos'),
('Hogar',           'Artículos para el hogar y decoración');

-- ============================================================
--  PROVEEDOR
-- ============================================================
INSERT INTO Proveedor (nombre_proveedor, telefono, correo, direccion, empresa) VALUES
('Carlos Martínez',  '7891-2345', 'carlos@techsv.com',    'Col. Escalón, San Salvador',      'TechSV S.A.'),
('Ana López',        '7823-4567', 'ana@distribuidora.com','Soyapango, San Salvador',          'Distribuidora López'),
('Roberto Flores',   '7856-7890', 'roberto@cleanpro.com', 'Santa Ana, Santa Ana',             'CleanPro'),
('María González',   '7834-5678', 'maria@papeleria.com',  'San Miguel, San Miguel',           'Papelería Central'),
('José Hernández',   '7812-3456', 'jose@herramientas.com','Mejicanos, San Salvador',          'HerraMax'),
('Laura Díaz',       '7867-8901', 'laura@modatextil.com', 'Antiguo Cuscatlán, La Libertad',   'Moda Textil S.A.'),
('Pedro Ramírez',    '7845-6789', 'pedro@farmasupply.com','Apopa, San Salvador',              'FarmaSupply'),
('Sofía Castro',     '7878-9012', 'sofia@jugueteria.com', 'Zacatecoluca, La Paz',             'Juguetería Alegría'),
('Miguel Torres',    '7801-2345', 'miguel@sportzone.com', 'Usulután, Usulután',               'SportZone'),
('Elena Morales',    '7889-0123', 'elena@hogardeco.com',  'Chalatenango, Chalatenango',       'HogarDeco');

-- ============================================================
--  MEDIDAS
-- ============================================================
INSERT INTO Medidas (unidad_medida) VALUES
('Unidad'),
('Kg'),
('Litro'),
('Caja'),
('Par'),
('Metro'),
('Paquete'),
('Gramo'),
('Docena'),
('Rollo');

-- ============================================================
--  ROLES
-- ============================================================
INSERT INTO Roles (nombre_rol) VALUES
('Administrador'),
('Vendedor'),
('Bodeguero');

-- ============================================================
--  MOTIVO MOVIMIENTO
-- ============================================================
INSERT INTO MotivoMovimiento (motivo) VALUES
('Compra a proveedor'),
('Venta a cliente'),
('Ajuste por inventario físico'),
('Devolución de cliente'),
('Devolución a proveedor'),
('Producto dañado'),
('Vencimiento de producto'),
('Transferencia entre bodegas'),
('Donación'),
('Error de registro');

-- ============================================================
--  PRODUCTO
-- ============================================================
INSERT INTO Producto (nombre_producto, descripcion, id_categoria, id_proveedor, id_medida, stock_actual, stock_minimo, fecha_caducidad, precio_compra, precio_venta, estado) VALUES
('Laptop HP 14"',         'Laptop 8GB RAM 256GB SSD',     1,  1,  1,  15,  5,  NULL,         450.00, 599.99, 'Activo'),
('Arroz Diana 1kg',       'Arroz blanco grano largo',     2,  2,  2,  200, 50, '2025-12-31', 0.75,   1.10,   'Activo'),
('Detergente Ariel 1L',   'Detergente líquido multiuso',  3,  3,  3,  80,  20, '2026-06-30', 2.50,   3.75,   'Activo'),
('Cuaderno universitario','100 hojas cuadriculado',       4,  4,  1,  150, 30, NULL,         1.20,   2.00,   'Activo'),
('Taladro Bosch 500W',    'Taladro eléctrico percutor',   5,  5,  1,  10,  3,  NULL,         65.00,  89.99,  'Activo'),
('Camisa polo talla M',   'Camisa polo 100% algodón',     6,  6,  1,  60,  10, NULL,         8.00,   14.99,  'Activo'),
('Paracetamol 500mg',     'Caja 20 tabletas',             7,  7,  4,  300, 100,'2025-09-30', 1.80,   3.00,   'Activo'),
('Set Lego 250 piezas',   'Set de construcción clásico',  8,  8,  1,  25,  5,  NULL,         18.00,  29.99,  'Activo'),
('Balón fútbol #5',       'Balón oficial FIFA',           9,  9,  1,  40,  10, NULL,         12.00,  19.99,  'Activo'),
('Lámpara LED escritorio','Lámpara ajustable 12W',        10, 10, 1,  35,  8,  NULL,         10.00,  17.50,  'Activo'),
('Teclado inalámbrico',   'Teclado Bluetooth compacto',   1,  1,  1,  20,  5,  NULL,         22.00,  34.99,  'Activo'),
('Aceite vegetal 1L',     'Aceite 100% vegetal',          2,  2,  3,  120, 40, '2025-11-30', 1.50,   2.25,   'Activo');

-- ============================================================
--  USUARIO
-- ============================================================
INSERT INTO Usuario (nombre, apellido, correo, contrasena, id_rol, estado) VALUES
('Admin',    'Sistema',    'admin@gestor.com',    'admin1234',    1, 'Activo'),
('Luis',     'Portillo',   'luis@gestor.com',     'vendedor123',  2, 'Activo'),
('Carmen',   'Vásquez',    'carmen@gestor.com',   'bodega1234',   3, 'Activo'),
('Ricardo',  'Fuentes',    'ricardo@gestor.com',  'vendedor456',  2, 'Activo'),
('Valentina','Orellana',   'vale@gestor.com',     'bodega5678',   3, 'Activo'),
('Diego',    'Montes',     'diego@gestor.com',    'vendedor789',  2, 'Inactivo'),
('Paola',    'Sandoval',   'paola@gestor.com',    'admin5678',    1, 'Activo'),
('Andrés',   'Guzmán',     'andres@gestor.com',   'bodega9012',   3, 'Activo'),
('Natalia',  'Rivas',      'natalia@gestor.com',  'vendedor012',  2, 'Activo'),
('Fernando', 'Aguilar',    'fernando@gestor.com', 'vendedor345',  2, 'Activo');

-- ============================================================
--  COMPRA
-- ============================================================
INSERT INTO Compra (fecha_compra, total_compra, id_usuario, estado) VALUES
('2024-01-10', 1350.00, 1, 'Completada'),
('2024-02-15', 480.00,  1, 'Completada'),
('2024-03-20', 225.00,  7, 'Completada'),
('2024-04-05', 540.00,  1, 'Completada'),
('2024-05-12', 360.00,  7, 'Completada'),
('2024-06-18', 195.00,  1, 'Completada'),
('2024-07-22', 890.00,  7, 'Completada'),
('2024-08-30', 420.00,  1, 'Completada'),
('2024-09-14', 275.00,  7, 'Anulada'),
('2024-10-25', 630.00,  1, 'Completada');

-- ============================================================
--  DETALLE COMPRA
-- ============================================================
INSERT INTO DetalleCompra (id_compra, id_producto, cantidad, precio_unitario, subtotal, comentario) VALUES
(1,  1,  3,  450.00, 1350.00, 'Pedido inicial de laptops'),
(2,  11, 10, 22.00,  220.00,  'Teclados para restock'),
(2,  10, 10, 10.00,  100.00,  NULL),
(3,  2,  100,0.75,   75.00,   'Arroz para stock mensual'),
(3,  6,  20, 1.50,   30.00,   NULL),
(4,  6,  30, 8.00,   240.00,  'Camisas temporada nueva'),
(4,  9,  25, 12.00,  300.00,  NULL),
(5,  3,  50, 2.50,   125.00,  NULL),
(5,  4,  80, 1.20,   96.00,   'Restock cuadernos'),
(6,  7,  100,1.80,   180.00,  'Medicamentos restock'),
(7,  5,  10, 65.00,  650.00,  'Taladros nuevos'),
(7,  8,  15, 18.00,  270.00,  NULL),  -- se corrige: 15*18=270, no 270
(8,  12, 100,1.50,   150.00,  NULL),
(8,  2,  180,0.75,   135.00,  'Restock arroz'),
(9,  1,  2,  450.00, 900.00,  'Compra anulada por error'),
(10, 10, 30, 10.00,  300.00,  NULL),
(10, 11, 15, 22.00,  330.00,  NULL);

-- ============================================================
--  VENTA
-- ============================================================
INSERT INTO Venta (fecha_venta, total_venta, id_usuario, estado) VALUES
('2024-01-15', 599.99,  2,  'Completada'),
('2024-02-20', 87.45,   4,  'Completada'),
('2024-03-10', 224.85,  2,  'Completada'),
('2024-04-08', 149.96,  9,  'Completada'),
('2024-05-17', 315.00,  4,  'Completada'),
('2024-06-25', 52.50,   2,  'Completada'),
('2024-07-30', 189.90,  10, 'Completada'),
('2024-08-12', 74.97,   9,  'Completada'),
('2024-09-05', 430.00,  4,  'Anulada'),
('2024-10-18', 262.44,  2,  'Completada');

-- ============================================================
--  DETALLE VENTA
-- ============================================================
INSERT INTO DetalleVenta (id_venta, id_producto, cantidad, precio_unitario, subtotal) VALUES
(1,  1,  1,  599.99, 599.99),
(2,  4,  3,  2.00,   6.00),
(2,  7,  5,  3.00,   15.00),
(2,  2,  10, 1.10,   11.00),
(3,  6,  5,  14.99,  74.95),
(3,  9,  3,  19.99,  59.97),
(3,  10, 5,  17.50,  87.50),
(4,  8,  5,  29.99,  149.95),
(5,  5,  1,  89.99,  89.99),
(5,  11, 1,  34.99,  34.99),
(5,  3,  5,  3.75,   18.75),
(6,  2,  20, 1.10,   22.00),
(6,  12, 10, 2.25,   22.50),
(7,  6,  6,  14.99,  89.94),
(7,  9,  5,  19.99,  99.95),
(8,  7,  10, 3.00,   30.00),
(8,  4,  5,  2.00,   10.00),
(9,  1,  1,  599.99, 599.99),
(10, 11, 3,  34.99,  104.97),
(10, 10, 5,  17.50,  87.50),
(10, 3,  10, 3.75,   37.50);

-- ============================================================
--  MOVIMIENTO INVENTARIO
-- ============================================================
INSERT INTO MovimientoInventario (id_producto, tipo_movimiento, cantidad, fecha_movimiento, id_motivo, comentario) VALUES
(1,  'Entrada', 3,   '2024-01-10', 1, 'Compra inicial laptops'),
(11, 'Entrada', 10,  '2024-02-15', 1, 'Restock teclados'),
(10, 'Entrada', 10,  '2024-02-15', 1, 'Restock lámparas'),
(2,  'Entrada', 100, '2024-03-20', 1, 'Compra arroz mensual'),
(1,  'Salida',  1,   '2024-01-15', 2, 'Venta cliente'),
(6,  'Salida',  5,   '2024-03-10', 2, 'Venta camisas'),
(9,  'Salida',  3,   '2024-03-10', 2, 'Venta balones'),
(7,  'Entrada', 100, '2024-06-18', 1, 'Restock medicamentos'),
(7,  'Salida',  15,  '2024-08-12', 2, 'Venta medicamentos'),
(5,  'Entrada', 10,  '2024-07-22', 1, 'Compra taladros'),
(5,  'Salida',  1,   '2024-05-17', 2, 'Venta taladro'),
(3,  'Salida',  15,  '2024-06-25', 2, 'Venta detergente'),
(12, 'Entrada', 100, '2024-08-30', 1, 'Compra aceite'),
(8,  'Salida',  5,   '2024-04-08', 2, 'Venta juguetes'),
(2,  'Salida',  1,   '2024-09-14', 6, 'Producto dañado en bodega'),
(4,  'Salida',  8,   '2024-02-20', 2, 'Venta cuadernos'),
(10, 'Entrada', 30,  '2024-10-25', 1, 'Restock lámparas'),
(11, 'Salida',  4,   '2024-10-18', 2, 'Venta teclados'),
(6,  'Entrada', 30,  '2024-04-05', 1, 'Compra camisas'),
(9,  'Entrada', 25,  '2024-04-05', 1, 'Compra balones');