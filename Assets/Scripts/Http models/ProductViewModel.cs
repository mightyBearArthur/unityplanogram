using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ProductViewModel
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
    /// Gets or sets the updated at.
    /// </summary>
    /// <value>
    /// The updated at.
    /// </value>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the additional properties.
    /// </summary>
    /// <value>
    /// The additional properties.
    /// </value>
    public string AdditionalProperties { get; set; }

    /// <summary>
    /// Gets or sets the desc image.
    /// </summary>
    /// <value>
    /// The desc image.
    /// </value>
    public byte[] Image { get; set; }

    /// <summary>
    /// Gets or sets the group identifier.
    /// </summary>
    /// <value>
    /// The group identifier.
    /// </value>
    public Guid ProductGroupId { get; set; }

    /// <summary>
    /// Gets or sets the recognition label.
    /// </summary>
    /// <value>
    /// The recognition label.
    /// </value>
    public string RecognitionLabel { get; set; }

}