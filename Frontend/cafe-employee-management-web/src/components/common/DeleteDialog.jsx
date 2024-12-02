import { Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Button } from '@mui/material';


const DeleteDialog = ({ isDialogOpen, handleCloseDialog, handleDelete }) => {
    return (<Dialog open={isDialogOpen} onClose={handleCloseDialog}>
        <DialogTitle>Confirm Deletion</DialogTitle>
        <DialogContent>
            <DialogContentText>
                Are you sure you want to delete this item? This action cannot be undone.
            </DialogContentText>
        </DialogContent>
        <DialogActions>
            <Button onClick={handleCloseDialog}>Cancel</Button>
            <Button onClick={handleDelete} color="error">
                Delete
            </Button>
        </DialogActions>
    </Dialog>);
}

export default DeleteDialog;