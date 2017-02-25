namespace TracingExperiment.Helpers
{
    /// <summary>
    /// An enumeration representing all of the options that can be set against a column.
    /// </summary>
    public enum ColumnOption
    {
        /// <summary>
        /// True to show a radio. The radio column has fixed width (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-radio")]
        radio,

        /// <summary>
        /// True to show a checkbox. The checkbox column has fixed width (defualt = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-checkbox")]
        checkbox,

        /// <summary>
        /// The column field name.
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-field")]
        field,

        /// <summary>
        /// The column title text.
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-title")]
        title,

        /// <summary>
        /// The column class name.
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-class")]
        @class,

        /// <summary>
        /// Indicate how to align the column data.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-align", Value = "left")]
        align_left,

        /// <summary>
        /// Indicate how to align the column data.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-align", Value = "right")]
        align_right,

        /// <summary>
        /// Indicate how to align the column data.
        /// </summary>
        /// <value>bool</value>        
        [ValueField(Name = "data-align", Value = "center")]
        align_center,

        /// <summary>
        /// Indicate how to align the table header.
        /// </summary>
        /// <value>bool</value>
        /// <remarks>1.4.0</remarks>
        [ValueField(Name = "data-halign", Value = "left")]
        halign_left,

        /// <summary>
        /// Indicate how to align the table header.
        /// </summary>
        /// <value>bool</value>
        /// <remarks>1.4.0</remarks>
        [ValueField(Name = "data-halign", Value = "right")]
        halign_right,

        /// <summary>
        /// Indicate how to align the table header.
        /// </summary>
        /// <value>bool</value>
        /// <remarks>1.4.0</remarks>
        [ValueField(Name = "data-halign", Value = "center")]
        halign_center,

        /// <summary>
        /// Indicate how to align the cell data.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-valign", Value = "top")]
        valign_top,

        /// <summary>
        /// Indicate how to align the cell data.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-valign", Value = "middle")]
        valign_middle,

        /// <summary>
        /// Indicate how to align the cell data.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-valign", Value = "bottom")]
        valign_bottom,

        /// <summary>
        /// The width of column. If not defined, the width will auto expand to fit its contents.
        /// </summary>
        /// <value>int</value>
        [NameField(Name = "data-width")]
        width,

        /// <summary>
        /// True to allow the column can be sorted (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-sortable")]
        sortable,

        /// <summary>
        /// The default sort order (default = asc).
        /// </summary>
        /// <value>string "asc" or "desc"</value>
        [NameField(Name = "data-order")]
        order,

        /// <summary>
        /// False to hide the columns item (default = true)
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-visible")]
        visible,

        /// <summary>
        /// False to disable the switchable of columns item (default = true).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-switchable")]
        switchable,

        /// <summary>
        /// True to select checkbox or radiobox when the column is clicked (default = true).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-click-to-select")]
        clickToSelect,

        /// <summary>
        /// The cell formatter function, take three parameters: value: the field value. row: 
        /// the row record data. index: the row index (function).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-formatter")]
        formatter,

        /// <summary>
        /// The cell events listener when you use formatter function, take three parameters: 
        /// event: the jQuery event. value: the field value. row: the row record data. index:
        /// the row index (function).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-events")]
        events,

        /// <summary>
        /// The custom field sort function that used to do local sorting, take two parameters:
        /// a: the first field value. b: the second field value (function).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-sorter")]
        sorter,

        /// <summary>
        /// The cell style formatter function, take three parameters: value: the field value.
        /// row: the row record data. index: the row index. Support classes or css (function).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-cell-style")]
        cellStyle,

        /// <summary>
        /// True to search data for this column.
        /// </summary>
        /// <value>bool</value>
        /// <remarks>1.5.0</remarks>
        [NameField(Name = "data-searchable")]
        searchable,
    }
}