migrationBuilder.AddColumn<int>(
    name: "Priority",
    table: "Tasks",
    type: "int",
    nullable: false,
    defaultValue: 1 // or your enum's default
);