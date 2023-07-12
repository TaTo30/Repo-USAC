# **Companias**

| Nombre      | Tipo         |
| ----------- | ------------ |
| No_Compania | INTEGER      |
| Nombre      | VARCHAR(100) |
| Contacto    | VARCHAR(100) |
| Correo      | VARCHAR(100) |
| Telefono    | VARCHAR(50)  |

# **Cliente o Proveedor**
| Nombre         | Tipo         |
| -------------- | ------------ |
| No_Entidad     | INTEGER      |
| Nombre         | VARHCAR(100) |
| Tipo           | CHAR         |
| Correo         | VARCHAR(100) |
| Telefono       | VARCHAR(50)  |
| Fecha_Registro | DATE         |
| *No_Direccion* | INTEGER      |

# **Regiones**
| Nombre    | Tipo        |
| --------- | ----------- |
| No_Region | INTEGER     |
| Nombre    | VARCHAR(50) |

# **Ciudades**
| Nombre      | Tipo        |
| ----------- | ----------- |
| No_Ciudad   | Integer     |
| Nombre      | VARCHAR(50) |
| *No_Region* | INTEGER     |

# **Direccion**
| Nombre        | Tipo         |
| ------------- | ------------ |
| No_Direccion  | INTEGER      |
| Direccion     | VARCHAR(100) |
| Codigo_Postal | INTEGER      |
| *No_Ciudad*   | INTEGER      |

# **Categoria**
| Nombre       | Tipo        |
| ------------ | ----------- |
| No_Categoria | INTEGER     |
| Nombre       | VARCHAR(50) |

# **Producto**
| Nombre          | Tipo         |
| --------------- | ------------ |
| No_Produto      | INTEGER      |
| Nombre          | VARCHAR(100) |
| Precio_Unitario | INTEGER      |
| *No_Categoria*  | INTEGER      |

# Compra o Venta
| Nombre         | Tipo    |
| -------------- | ------- |
| No_Transaccion | INTEGER |
| Tipo           | CHAR    |
| Cantidad       | INTEGER |
| *No_Producto*  | INTEGER |
| *No_Entidad*   | INTEGER |
| *No_Compania*  | INTEGER |

