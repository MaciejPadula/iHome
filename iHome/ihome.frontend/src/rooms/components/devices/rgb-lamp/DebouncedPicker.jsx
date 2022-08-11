import React, { useState } from "react";
import { useDebouncyEffect } from "use-debouncy";
import { HexColorPicker } from "react-colorful";

const DebouncedPicker = ({ color, onChange }) => {
  const [value, setValue] = useState(color);

  useDebouncyEffect(() => onChange(value), 200, [value]);

  return <HexColorPicker color={value} onChange={setValue} />;
};

export default DebouncedPicker;