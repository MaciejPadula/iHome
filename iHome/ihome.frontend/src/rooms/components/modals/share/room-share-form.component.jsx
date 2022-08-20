import React, { useState, useEffect, useId, useRef } from "react";

//api
import { shareRoom, getUsersEmails } from '../../../api/apiRequests';

//components
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import FloatingLabel from 'react-bootstrap/FloatingLabel';
import InputGroup from 'react-bootstrap/InputGroup';
import { ShareFill } from "react-bootstrap-icons";
import { Typeahead, AsyncTypeahead } from 'react-bootstrap-typeahead';

const ShareRoomForm = ({roomId, onSubmit, ...props}) => {
    const emailInput = useRef(null);
    const getEmail = () => emailInput.current.inputNode.value;
    const clearEmail = () => console.log(emailInput.current.inputNode.value="");

    const searchId = useId();

    const [isLoading, setIsLoading] = useState(false);
    const [options, setOptions] = useState([]);

    const handleSearch = (query) => {
        setIsLoading(true);
        getUsersEmails(query).then(r => {
             setOptions(r.data);
        });
        setIsLoading(false);
    };

    const filterBy = () => true;

    const ShareRoom = (ev) => {
        const email = getEmail();
        
        shareRoom(roomId, email).then(res => {
            if(res.data.status == 200){
                clearEmail();
                onSubmit();
                
            }
            else if(res.data.status == 801){
                console.log(res.data.exception)
            }
        });
    };

    return (
        <InputGroup className="mb-3">
            <AsyncTypeahead
                ref={emailInput}
                id={searchId}
                filterBy={filterBy}
                isLoading={isLoading}
                labelKey="email"
                minLength={3}
                onSearch={handleSearch}
                options={options}
                placeholder="Enter friend's email"
                name="email"
                defaultValue=""
                required
            />
            <Button variant="primary" type="submit" onClick={ShareRoom}>
                <ShareFill />
            </Button>
            <Form.Control.Feedback type="invalid">
                Invalid email!
            </Form.Control.Feedback>
        </InputGroup>
    );
};

export default ShareRoomForm;