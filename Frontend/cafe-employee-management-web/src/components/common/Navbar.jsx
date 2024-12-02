import React from 'react';
import { AppBar, Toolbar, Typography, Button } from '@mui/material';
import { Link } from 'react-router-dom';

const Navbar = () => {
    return (
        <AppBar position="static">
            <Toolbar>
                <Typography variant="h6" style={{ flexGrow: 1 }}>
                    Cafe Employee Management
                </Typography>
                <Button color="inherit" component={Link} to="/">
                    Cafes
                </Button>
                <Button color="inherit" component={Link} to="/employees">
                    Employees
                </Button>
            </Toolbar>
        </AppBar>
    );
};

export default Navbar;
