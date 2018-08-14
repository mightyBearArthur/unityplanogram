using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlanogramCellViewModel
{
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the created at.
    /// </summary>
    /// <value>
    /// The created at.
    /// </value>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the updated at.
    /// </summary>
    /// <value>
    /// The updated at.
    /// </value>
    public DateTime UpdatedAt { get; set; }
    /// <summary>
    /// Gets or sets the row.
    /// </summary>
    /// <value>
    /// The row.
    /// </value>
    public int Row { get; set; }
    /// <summary>
    /// Gets or sets the column.
    /// </summary>
    /// <value>
    /// The column.
    /// </value>
    public int Column { get; set; }


    /// <summary>
    /// Gets or sets the product identifier.
    /// </summary>
    /// <value>
    /// The product identifier.
    /// </value>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    /// <value>
    /// The name of the product.
    /// </value>
    public string ProductName { get; set; }

    /// <summary>
    /// Gets or sets the product description.
    /// </summary>
    /// <value>
    /// The product description.
    /// </value>
    public string ProductDescription { get; set; }

    /// <summary>
    /// Gets or sets the product image.
    /// </summary>
    /// <value>
    /// The product image.
    /// </value>
    public byte[] ProductImage { get; set; }
}