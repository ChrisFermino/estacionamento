import React from 'react';
import {Button, Col, DatePicker, Form, Input, Modal, Row} from 'antd';

// eslint-disable-next-line react/prop-types
const CheckOutModal = ({visible, onClose, onSubmit}) => {
    const [form] = Form.useForm();

    const handleSubmit = () => {
        form.validateFields().then((values) => {
            console.log('cheguei aqui')
            form.resetFields();
            onSubmit(values);
        });
    };

    return (
        <Modal title="Saída de Veículo" open={visible} onCancel={onClose} footer={null}>
            <Form form={form} layout="vertical" onFinish={handleSubmit}>
                <Row gutter={24}>
                    <Col span={24}>
                        <Form.Item label="Placa" name="plate"
                                   rules={[{required: true, message: 'Informe a placa do veículo'}]}>
                            <Input/>
                        </Form.Item>
                    </Col>
                    <Col span={24}>
                        <Form.Item label="Data e Hora de Saída" name="checkOutDate" rules={[{required: true,}]}>
                            <DatePicker showTime format="YYYY-MM-DD HH:mm:ss"/>
                        </Form.Item>
                    </Col>
                    <Button type="primary" htmlType="submit">Registrar Saída</Button>
                </Row>
            </Form>
        </Modal>
    );
};

export default CheckOutModal;
