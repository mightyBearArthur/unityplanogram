using System.Collections;
using System.Collections.Generic;
using System;

public class BackwallViewModel
{

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the rows count.
    /// </summary>
    /// <value>
    /// The rows count.
    /// </value>
    public int RowsCount { get; set; }

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
    /// Gets or sets the cells.
    /// </summary>
    /// <value>
    /// The cells.
    /// </value>
    public List<List<BackwallCellViewModel>> Cells { get; set; }

}
