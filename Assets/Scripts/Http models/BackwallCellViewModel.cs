using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BackwallCellViewModel
{

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the created at.
    /// </summary>
    /// <value>
    /// The created at.
    /// </value>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the planogram template identifier.
    /// </summary>
    /// <value>
    /// The planogram template identifier.
    /// </value>
    public Guid PlanogramTemplateId { get; set; }
    /// <summary>
    /// Gets or sets the canvas x.
    /// </summary>
    /// <value>
    /// The canvas x.
    /// </value>
    public double CanvasX { get; set; }
    /// <summary>
    /// Gets or sets the canvas y.
    /// </summary>
    /// <value>
    /// The canvas y.
    /// </value>
    public double CanvasY { get; set; }
    /// <summary>
    /// Gets or sets the width of the canvas.
    /// </summary>
    /// <value>
    /// The width of the canvas.
    /// </value>
    public double CanvasWidth { get; set; }
    /// <summary>
    /// Gets or sets the height of the canvas.
    /// </summary>
    /// <value>
    /// The height of the canvas.
    /// </value>
    public double CanvasHeight { get; set; }
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

}
