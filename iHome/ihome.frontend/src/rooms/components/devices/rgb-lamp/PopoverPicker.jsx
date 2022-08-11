import React, { useCallback, useRef, useState } from "react";
import { HexColorPicker } from "react-colorful";
import DebouncedPicker from "./DebouncedPicker";

import OverlayTrigger from "react-bootstrap/OverlayTrigger";
import Popover from "react-bootstrap/Popover";
import useClickOutside from "./useClickOutside";

const PopoverPicker = ({ color, onChange }) => {
  const popover = useRef();
  const [isOpen, toggle] = useState(false);

  const close = useCallback(() => toggle(false), []);
  useClickOutside(popover, close);

  return (
    <OverlayTrigger
      trigger="click"
      placement="bottom"
      overlay={isOpen && (
            <Popover>
              <Popover.Body>
                <DebouncedPicker color={color} onChange={onChange} />
              </Popover.Body>
            </Popover>
      )}
      rootClose
    >
      <div
        className="switch"
        style={{ backgroundColor: color, width:"5rem", height:"2rem", margin:"0.5rem", borderRadius: "5%" }}
        onClick={() => toggle(true)}
      />
    </OverlayTrigger>
    // <div className="picker">
    //   <div
    //     className="switch"
    //     style={{ backgroundColor: color, width:"5rem", height:"2rem", margin:"0.5rem", borderRadius: "5%" }}
    //     onClick={() => toggle(true)}
    //   />

    //   {isOpen && (
    //     <Popover>
    //       <Popover.Body>
    //         <DebouncedPicker color={color} onChange={onChange} />
    //       </Popover.Body>
    //     </Popover>
    //   )}
    // </div>
  );
};
export default PopoverPicker;