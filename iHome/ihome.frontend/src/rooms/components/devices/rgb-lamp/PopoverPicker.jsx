import React, { useCallback, useRef, useState } from "react";
import { HexColorPicker } from "react-colorful";
import DebouncedPicker from "./DebouncedPicker";

import useClickOutside from "./useClickOutside";

const PopoverPicker = ({ color, onChange }) => {
  const popover = useRef();
  const [isOpen, toggle] = useState(false);

  const close = useCallback(() => toggle(false), []);
  useClickOutside(popover, close);

  return (
    <div className="picker">
      <div
        className="switch"
        style={{ backgroundColor: color, width:"5rem", height:"2rem", margin:"0.5rem", borderRadius: "5%" }}
        onClick={() => toggle(true)}
      />

      {isOpen && (
        <div className="popover" ref={popover}>
          <DebouncedPicker color={color} onChange={onChange} />
        </div>
      )}
    </div>
  );
};
export default PopoverPicker;