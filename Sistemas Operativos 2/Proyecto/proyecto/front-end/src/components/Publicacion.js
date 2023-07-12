import React, { useState, useRef, useEffect } from "react";
import { Chip } from "primereact/chip";
import { Image } from "primereact/image";
import { Divider } from "primereact/divider";
import { Card } from "primereact/card";

const Publicacion = ({ data = {} }) => {
    return (
        <Card className="card">
            <Chip label={data.date !== undefined ? data.date : "00/00/0000-00:00:00"} icon={"pi pi-calendar"} />
            <br />
            <br />
            <Chip label={`@ ${data.username !== undefined ? data.username : "No-Username"}`} className="mr-2 mb-2 custom-chip" />
            <h5>{data.name !== undefined ? data.name : "No-Name"}</h5>
            <div className="flex">
                <Image src={data.image} alt="image-publicacion" width={250} preview />
                <Divider layout="vertical">
                    <b>Pub</b>
                </Divider>

                <p>{data.text !== undefined ? data.text : "There is no description"}</p>
            </div>
        </Card>
    );
};

export default Publicacion;
