import Axios from "axios";
import { Button } from "primereact/button";
import { Image } from "primereact/image";
import { ListBox } from "primereact/listbox";
import { Toast } from "primereact/toast";

import React, { useEffect, useState, useRef } from "react";

const ListaAlbums = () => {
    const toast = useRef();

    const [baseUrl, setBaseUrl] = useState("http://application-load-balancer-1846011889.us-east-2.elb.amazonaws.com");
    const [listboxValue, setListboxValue] = useState(null);
    const [listboxValues, setListboxValues] = useState([{ album: "No albums" }]);
    const [fotosAlbumActual, setFotosAlbumActual] = useState([]);
    const [selectedItem, setSelectedItem] = useState("");

    const [userData, setUserData] = useState(() => {
        // getting stored value
        const saved = window.localStorage.getItem("key");
        const initialValue = JSON.parse(saved);
        return initialValue || {};
    });

    const showWarn = (causa, mensaje) => {
        toast.current.show({ severity: "warn", summary: causa, detail: mensaje, life: 6000 });
    };

    const showSuccess = (causa, mensaje) => {
        toast.current.show({ severity: "success", summary: causa, detail: mensaje, life: 6000 });
    };

    const getListaAlbums = async () => {
        try {
            await Axios.get(`${baseUrl}/albums/${userData.id}`)
                .then((res) => {
                    let albums = res.data.albums.map((element) => {
                        return {
                            album: element[0],
                        };
                    });
                    setListboxValues(albums);
                    console.table(albums);
                })
                .catch((err) => {
                    console.log(err);
                    showWarn("API Connection", "Ha ocurrido un error al hacer la peticion lista de albums, " + err);
                });
        } catch (error) {
            console.error(error);
            showWarn("API Connection", "Ha ocurrido un error al hacer la peticion, " + error);
        }
    };

    const getListaFotosFromAlbum = async () => {
        if (selectedItem === "") {
            return;
        }
        console.log("Getting image from album:", selectedItem);
        let body = {
            idUsuario: userData.id,
            nameAlbum: selectedItem,
        };
        console.dir(body);
        try {
            await Axios.post(`${baseUrl}/getListaFotos`, body)
                .then((res) => {
                    let fotos = res.data.fotos.map((element) => {
                        return element[0];
                    });
                    console.log({ fotos });
                    setFotosAlbumActual(fotos);
                    console.log("Fotos obtenidas, next...");
                })
                .catch((err) => {
                    console.log("Something went wrong,", err);
                });
        } catch (error) {
            console.error(error);
            showWarn("API Connection", "Ha ocurrido un error al hacer la peticion, " + error);
        }
    };

    const eliminarImagen = async (img) => {
        let body = {
            idUsuario: userData.id,
            nameAlbum: selectedItem,
            foto: img,
        };
        console.dir(body);
        try {
            await Axios.delete(`${baseUrl}/deleteFoto`, { data: body })
                .then((res) => {
                    console.log(res.data.msg);
                    getListaFotosFromAlbum();
                    showSuccess("Eliminar Imagen", res.data.msg);
                })
                .catch((err) => {
                    showWarn("API Connection", "Algo ha ocurrido al tratar de hacer la peticion:" + err);
                });
        } catch (error) {
            console.error(error);
            showWarn("API Connection", "Ha ocurrido un error al hacer la peticion, " + error);
        }
    };

    const handleListBoxValueSelected = (e) => {
        setListboxValue(e.value);
    };

    const bottonImageAction = async (e) => {
        await eliminarImagen(e.target.dataset.srcimg.toString());
    };

    useEffect(() => {
        if (listboxValue !== null) {
            setSelectedItem(listboxValue.album);
        } else {
            setSelectedItem("");
        }
    }, [listboxValue]);

    useEffect(() => {
        getListaFotosFromAlbum();
    }, [selectedItem]);

    useEffect(() => {
        getListaAlbums();
    }, []);

    return (
        <div className="grid">
            <div className="col-12">
                <div className="card">
                    <div className="grid p-fluid">
                        <Toast ref={toast} />

                        <h5>Mis Albums</h5>
                        <div className="col-12">
                            <ListBox value={listboxValue} onChange={handleListBoxValueSelected} options={listboxValues} optionLabel={"album"} filter />
                        </div>
                        <div className="col-12">
                            {fotosAlbumActual.length !== 0 ? (
                                <div className="card p-fluid">
                                    {fotosAlbumActual.map((imagen, index) => (
                                        <>
                                            <h6>Imagen No.0{index}</h6>
                                            <Image src={imagen} width={250} preview></Image>
                                            <div className="col-12 md:col-6">
                                                <Button data-srcimg={imagen} icon="pi pi-trash" onClick={bottonImageAction} label="Eliminar" className="mr-2"></Button>
                                            </div>
                                        </>
                                    ))}
                                </div>
                            ) : (
                                <div>
                                    <h5>Sin fotos...</h5>
                                    <h4>:'(</h4>
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ListaAlbums;
