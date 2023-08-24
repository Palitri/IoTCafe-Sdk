package com.palitri.openiot.construction.framework.composite;

import com.palitri.openiot.construction.framework.board.models.BoardProperty;
import com.palitri.openiot.construction.framework.web.models.configurations.project.Peripheral;
import com.palitri.openiot.construction.framework.web.models.configurations.project.PeripheralProperty;

public class CompositeProperty {
    public BoardProperty boardProperty;
    public PeripheralProperty peripheralProperty;
    public Peripheral parentPeripheral;
}
