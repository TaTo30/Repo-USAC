import React, { useRef } from "react";
import { Link, useHistory } from "react-router-dom";
import classNames from "classnames";
import { Button } from "primereact/button";
import { confirmPopup } from 'primereact/confirmpopup';
import { Toast } from 'primereact/toast';


export const AppTopbar = (props) => {
    let history = useHistory();
    const toast = useRef(null);

    const accept = () => {
        window.localStorage.removeItem("key")
        history.push("/");
    };

    const reject = () => {
        toast.current.show({ severity: 'error', summary: 'Rejected', detail: 'You have rejected', life: 3000 });
    };

    const confirm = (event) => {
        confirmPopup({
            target: event.currentTarget,
            message: 'Are you sure you want to proceed?',
            icon: 'pi pi-exclamation-triangle',
            accept,
            reject
        });

    };

    return (
        <div className="layout-topbar">
            <Toast ref={toast} />
            <Link to="/" className="layout-topbar-logo">
                <span>Proyecto 2 Sistemas Operativos</span>
            </Link>

            <button type="button" className="p-link  layout-menu-button layout-topbar-button" onClick={props.onToggleMenuClick}>
                <i className="pi pi-bars" />
            </button>

            <button type="button" className="p-link layout-topbar-menu-button layout-topbar-button" onClick={props.onMobileTopbarMenuClick}>
                <i className="pi pi-ellipsis-v" />
            </button>

            <ul className={classNames("layout-topbar-menu lg:flex origin-top", { "layout-topbar-menu-mobile-active": props.mobileTopbarMenuActive })}>
                <li>
                    <button className="p-link layout-topbar-button" onClick={props.onMobileSubTopbarMenuClick}>
                        <i className="pi pi-user" />
                        <span>Profile</span>
                    </button>
                </li>
                <li>
                    <Button onClick={confirm} icon="pi pi-power-off" label="Logout" className="mr-2"></Button>
                </li>
            </ul>
        </div>
    );
};
