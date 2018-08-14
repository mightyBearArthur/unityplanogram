using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlanogramViewModel
{

    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the planogram template identifier.
    /// </summary>
    /// <value>
    /// The planogram template identifier.
    /// </value>
    public Guid PlanogramTemplateId { get; set; }

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
    /// Gets or sets the cells.
    /// </summary>
    /// <value>
    /// The cells.
    /// </value>
    //[Required]
    public List<PlanogramCellViewModel> Cells { get; set; }

}